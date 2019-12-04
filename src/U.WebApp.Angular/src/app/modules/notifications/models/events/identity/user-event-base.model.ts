import {BaseEvent} from "../base-event.model";

export interface UserEventBase extends BaseEvent
{
  UserId: string;
  Nickname: string;
  Role: string;
}
