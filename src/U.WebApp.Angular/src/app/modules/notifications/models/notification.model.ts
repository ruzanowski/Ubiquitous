export interface NotificationDto<T>
{
  Event: T;
  Id: string;
  EventType: IntegrationEventType;
  CreationTime: Date;
}

export enum IntegrationEventType
{
  Unknown,
  ProductPublishedIntegrationEvent,
  ProductPropertiesChangedIntegrationEvent,
  ProductAddedIntegrationEvent
}
