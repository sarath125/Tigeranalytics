using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailPrice.DTO;
using RetailPrice.Models;
using RetailPrice.Utility;
using System.Globalization;
using System.Linq.Expressions;

namespace RetailPrice.Business.PricingService
{
    public class PricingService : IPricingService
    {
        private readonly RetailPriceContext _context;
        private readonly IMapper _mapper;

        public PricingService(RetailPriceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<PricingDtoView>> GetAllPricingsAsync([FromBody] GridParameters gridParams)
        {
            try
            {

                var query = _context.Pricings.Include(p => p.Store).AsQueryable();


                // Apply filters
                if (gridParams.FilterModel != null && gridParams.FilterModel.Any())
                {
                    query = GridUtility.ApplyFilters(query, gridParams.FilterModel);
                }

                // Apply grouping if required
                if (gridParams.RowGroupCols != null && gridParams.RowGroupCols.Any())
                {
                    query = ApplyGrouping(query, gridParams.RowGroupCols);
                }

                // Apply sorting
                // query = GridUtility.ApplySorting(query, gridParams.SortModel);

                // Get total count for pagination
                int totalCount = await query.CountAsync();

                // Apply pagination
                int pageSize = gridParams.EndRow - gridParams.StartRow;
                query = query.Skip(gridParams.StartRow).Take(pageSize);

                var pricings = await query.ToListAsync();

                var dtoPricings = pricings.Select(p => new PricingDtoView
                {
                    PricingID = p.PricingId,
                    StoreName = p.Store?.StoreName,
                    Sku = p.Sku,
                    Price = p.Price,
                    Date = p.Date,
                    Country = p.Store?.Country,
                    StoreId = p.StoreId
                }).ToList();


                var pagedResult = new PagedResult<PricingDtoView>(
           dtoPricings,    // The data items for the current page
           totalCount    // Total number of items across all pages

       );






                return pagedResult;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        private IQueryable<Pricing> ApplyGrouping(IQueryable<Pricing> query, List<ColumnModel> groupCols)
        {
            // Check if any grouping is required
            if (groupCols == null || !groupCols.Any())
            {
                return query; // No grouping needed
            }

            // Dynamically build the key for grouping
            var groupingKeySelectors = new List<Expression<Func<Pricing, object>>>();

            if (groupCols.Any(gc => gc.Field == "StoreName"))
            {
                groupingKeySelectors.Add(p => p.Store.StoreName);
            }
            if (groupCols.Any(gc => gc.Field == "Country"))
            {
                groupingKeySelectors.Add(p => p.Store.Country);
            }
            if (groupCols.Any(gc => gc.Field == "Date"))
            {
                groupingKeySelectors.Add(p => p.Date);
            }

            // Apply grouping based on the constructed key
            // Note: This part is tricky because EF Core doesn't directly support dynamic multi-key grouping
            // The following is a conceptual representation and might need actual implementation adjustments
            if (groupingKeySelectors.Count > 0)
            {
                query = groupingKeySelectors
                    .Aggregate(query, (current, selector) => current.GroupBy(selector).SelectMany(g => g));
            }

            return query;
        }




        public async Task<PricingDto> GetPricingByIdAsync(int id)
        {
            var pricing = await _context.Pricings.FindAsync(id);
            return _mapper.Map<PricingDto>(pricing);
        }

        public async Task AddPricingAsync(PricingDto pricingDto)
        {
            var pricing = _mapper.Map<Pricing>(pricingDto);
            _context.Pricings.Add(pricing);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePricingAsync(PricingDto pricingDto)
        {
            var pricing = _mapper.Map<Pricing>(pricingDto);
            _context.Pricings.Update(pricing);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePricingAsync(int id)
        {
            var pricing = await _context.Pricings.FindAsync(id);
            if (pricing != null)
            {
                _context.Pricings.Remove(pricing);
                await _context.SaveChangesAsync();
            }
        }

        public void ProcessSKUPriceFile(string filePath)
        {


            using (var reader = new StreamReader(filePath))

            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                IgnoreBlankLines = true,
            }))
            {
                csv.Context.RegisterClassMap<PricingCsvRecordMap>();

                var records = csv.GetRecords<PricingCsvRecord>().ToList();




                foreach (var record in records)
                {




                    // Validate StoreID and SKU
                    if (_context.Stores.Any(s => s.StoreId == record.StoreID) &&
                        _context.Products.Any(p => p.Sku == record.SKU))
                    {
                        // Check if a record already exists
                        var existingRecord = _context.Pricings
                            .FirstOrDefault(p => p.StoreId == record.StoreID && p.Sku == record.SKU && p.Date == record.Date);

                        if (existingRecord != null)
                        {
                            // Update existing record
                            existingRecord.Price = record.Price;
                        }
                        else
                        {
                            // Insert new record
                            var newPricing = new Pricing
                            {
                                StoreId = record.StoreID,
                                Sku = record.SKU,
                                Price = record.Price,
                                Date = record.Date
                            };
                            _context.Pricings.Add(newPricing);
                        }
                    }
                    // Else, skip the record
                }

                _context.SaveChanges();

            }

        }
    }
}
