import {IntegrationEventType} from "./integration-event-type.model";

export interface NotificationCreationStatistics
{
  dateTime: Date;
  count: number;
}
export interface NotificationEventTypeStatistics
{
  eventType: IntegrationEventType;
  count: number;
}
