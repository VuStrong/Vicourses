import User from "../entity/user.entity";
import * as rabbitmq from "../rabbitmq/client";

export default async function onUserRoleUpdated(user: User) {
    rabbitmq.publishUserRoleUpdated(user);
}