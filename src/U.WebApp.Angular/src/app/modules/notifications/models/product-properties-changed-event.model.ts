import {ProductBaseEvent} from "./product-base-event.model";

export interface ProductPropertiesChangedEvent extends ProductBaseEvent
{
  Price: number;
}
