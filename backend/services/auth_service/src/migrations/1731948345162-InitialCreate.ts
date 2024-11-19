import { MigrationInterface, QueryRunner } from "typeorm";

export class InitialCreate1731948345162 implements MigrationInterface {
    name = 'InitialCreate1731948345162'

    public async up(queryRunner: QueryRunner): Promise<void> {
        await queryRunner.query(`CREATE TABLE \`users\` (\`id\` varchar(36) NOT NULL, \`createdAt\` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6), \`name\` varchar(255) NOT NULL, \`email\` varchar(255) NOT NULL, \`passwordHash\` varchar(255) NOT NULL, \`emailConfirmed\` tinyint NOT NULL DEFAULT 0, \`lockoutEnd\` datetime NULL, \`role\` enum ('admin', 'instructor', 'student') NOT NULL DEFAULT 'student', UNIQUE INDEX \`IDX_97672ac88f789774dd47f7c8be\` (\`email\`), PRIMARY KEY (\`id\`)) ENGINE=InnoDB`);
        await queryRunner.query(`CREATE TABLE \`tokens\` (\`token\` varchar(255) NOT NULL, \`userId\` varchar(255) NOT NULL, \`type\` enum ('refresh_token', 'comfirm_email', 'reset_password') NOT NULL, \`expiryTime\` datetime NOT NULL, INDEX \`IDX_d417e5d35f2434afc4bd48cb4d\` (\`userId\`), PRIMARY KEY (\`token\`, \`userId\`)) ENGINE=InnoDB`);
    }

    public async down(queryRunner: QueryRunner): Promise<void> {
        await queryRunner.query(`DROP INDEX \`IDX_d417e5d35f2434afc4bd48cb4d\` ON \`tokens\``);
        await queryRunner.query(`DROP TABLE \`tokens\``);
        await queryRunner.query(`DROP INDEX \`IDX_97672ac88f789774dd47f7c8be\` ON \`users\``);
        await queryRunner.query(`DROP TABLE \`users\``);
    }

}
