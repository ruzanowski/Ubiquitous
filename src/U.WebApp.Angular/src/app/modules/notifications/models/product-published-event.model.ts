import {ProductBaseEvent} from "./product-base-event.model";

export interface ProductPublishedEvent extends ProductBaseEvent
{

  Price: number;
  Manufacturer: string;
}



