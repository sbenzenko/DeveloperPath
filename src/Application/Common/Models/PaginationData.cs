using System;

namespace DeveloperPath.Application.Common.Models
{
  /// <summary>
  /// Class for adding pagination data to output headers
  /// </summary>
  public class PaginationData
  {
    /// <summary>
    /// Page number
    /// </summary>
    public int PageNumber { get; set; }
    /// <summary>
    /// Items per page
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// Link to first page
    /// </summary>
    public Uri FirstPage { get; set; }
    /// <summary>
    /// Link to next page
    /// </summary>
    public Uri NextPage { get; set; }
    /// <summary>
    /// Link to previous page
    /// </summary>
    public Uri PreviousPage { get; set; }
    /// <summary>
    /// Link to last page
    /// </summary>
    public Uri LastPage { get; set; }
    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; set; }
    /// <summary>
    /// Total number of items
    /// </summary>
    public int TotalRecords { get; set; }

    /// <summary>
    /// Creates simple pagination object
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    public PaginationData(int pageNumber, int pageSize)
    {
      PageNumber = pageNumber;
      PageSize = pageSize;
    }
  }
}
