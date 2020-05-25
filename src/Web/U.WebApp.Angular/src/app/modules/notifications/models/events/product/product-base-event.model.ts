import {BaseEvent} from "../base-event.model";

export interface ProductBaseEvent extends  BaseEvent
{
  id: string;
  productId: string;
  name: string;
  creationDate: Date;
  manufacturer: string;
}
