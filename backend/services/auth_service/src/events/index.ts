import EventEmitter from "events";
import onUserCreated from "./user-created.event";
import onUserRoleUpdated from "./user-role-updated.event";

export enum EventType {
    USER_CREATED = "user_created",
    USER_ROLE_UPDATED = "user_role_updated"
}
export const event = new EventEmitter();

event.on(EventType.USER_CREATED, onUserCreated);
event.on(EventType.USER_ROLE_UPDATED, onUserRoleUpdated);