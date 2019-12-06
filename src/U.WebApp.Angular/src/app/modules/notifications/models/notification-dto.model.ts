import {ConfirmationType} from "./confirmation-type.model";
import {IntegrationEventType} from "./integration-event-type.model";
import {Importancy} from "./importancy.model";

export interface NotificationDto<T> {
  event: T;
  id: string;
  eventType: IntegrationEventType;
  creationTime: Date;
  state: ConfirmationType;
  importancy: Importancy;
}



