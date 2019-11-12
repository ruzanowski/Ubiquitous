import {ProductBaseEvent} from "./product-base-event.model";
import {Variance} from "./variance.model";

export interface ProductPropertiesChangedEvent extends ProductBaseEvent
{
  variances: Array<Variance>;
}

