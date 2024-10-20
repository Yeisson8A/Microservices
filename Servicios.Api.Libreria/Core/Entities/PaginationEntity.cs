﻿namespace Servicios.Api.Libreria.Core.Entities
{
    public class PaginationEntity<TDocument>
    {
        public int PageSize { get; set; }

        public int Page {  get; set; }

        public string Sort { get; set; }

        public string SortDirection { get; set; }

        public FilterValue? FilterValue { get; set; }

        public int PagesQuantity { get; set; }

        public int TotalRows { get; set; }

        public IEnumerable<TDocument>? Data { get; set; }
    }
}
