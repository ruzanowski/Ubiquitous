export interface ProductStatistics
{
  dateTime: Date;
  count: number;
}

export interface ProductStatisticsByCategory
{
  categoryName: string;
  count: number;
}


export interface ProductStatisticsByManufacturer
{
  manufacturerName: string;
  count: number;
}
