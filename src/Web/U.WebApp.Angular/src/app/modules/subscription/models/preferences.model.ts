import {Importancy} from "../../notifications/models/importancy.model";

export interface Preferences
{
  numberOfWelcomeMessages: number;
  doNotNotifyAnyoneAboutMyActivity: boolean;
  orderByCreationTimeDescending: boolean;
  seeReadNotifications: boolean;
  seeUnreadNotifications: boolean;
  minimalImportancyLevel: Importancy;
}

