export interface PaginatedItems<T> {
  data: Array<T>;
  pageIndex: number;
  pageSize: number;
}
