import {ProductBaseEvent} from "./product-base-event.model";

export interface ProductAddedEvent extends ProductBaseEvent
{
  Price: number;
}
