import {Category} from "../../categories/models/category.model";

export interface Product {
  id: string;
  createdDateTime: string;
  lastFullUpdateDateTime: string;
  name: string;
  barCode: string;
  price: number;
  description: string;
  IsPublished: boolean;
  dimensions: Dimensions;
  manufacturerId: string;
  category: Category;
}
export interface ProductStatistics
{
  dateTime: Date;
  count: number;
}
