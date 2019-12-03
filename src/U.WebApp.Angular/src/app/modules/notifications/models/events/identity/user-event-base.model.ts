import {BaseEvent} from "./user-connected.model";

export interface UserEventBase extends BaseEvent
{
  UserId: string;
  Nickname: string;
  Role: string;
}
