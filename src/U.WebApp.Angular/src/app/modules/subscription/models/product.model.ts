import {Importancy} from "../../notifications/models/importancy.model";
import {IntegrationEventType} from "../../notifications/models/integration-event-type.model";

export interface Preferences
{
  NumberOfWelcomeMessages: number;
  DoNotNotifyAnyoneAboutMyActivity: boolean;
  OrderByCreationTimeDescending: boolean;
  SeeReadNotifications: boolean;
  SeeUnreadNotifications: boolean;
  MinimalImportancyLevel: Importancy;

}

export interface AllowedEvents
{
    events: IntegrationEventType[];
}
