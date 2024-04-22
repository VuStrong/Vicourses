import EventEmitter from "events";
import onUserCreated from "./user-created.event";

export enum EventType {
    USER_CREATED = "user_created"
}
export const event = new EventEmitter();

event.on(EventType.USER_CREATED, onUserCreated);