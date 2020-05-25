import {BaseEvent} from "../base-event.model";

export interface UserEventBase extends BaseEvent
{
  userId: string;
  nickname: string;
  role: string;
}
