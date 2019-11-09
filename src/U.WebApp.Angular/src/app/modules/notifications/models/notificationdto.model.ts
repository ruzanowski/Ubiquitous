export interface NotificationDto<T>
{
  event: T;
  id: string;
  eventType: IntegrationEventType;
  creationTime: Date;
  state: ConfirmationType;
}

export enum IntegrationEventType
{
  Unknown = 0,
  ProductPublishedIntegrationEvent = 1,
  ProductPropertiesChangedIntegrationEvent = 2,
  ProductAddedIntegrationEvent = 3
}

export enum ConfirmationType
{
  unread = 0,
  read = 1,
  removed = 2,
  hidden = 3
}
