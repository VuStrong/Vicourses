import { MigrationInterface, QueryRunner } from "typeorm";

export class InitialCreate1732958187637 implements MigrationInterface {
    name = 'InitialCreate1732958187637'

    public async up(queryRunner: QueryRunner): Promise<void> {
        await queryRunner.query(`CREATE TABLE \`refresh_tokens\` (\`userId\` varchar(255) NOT NULL, \`token\` varchar(255) NOT NULL, \`expiryTime\` datetime NOT NULL, PRIMARY KEY (\`userId\`, \`token\`)) ENGINE=InnoDB`);
        await queryRunner.query(`CREATE TABLE \`paypal_accounts\` (\`id\` varchar(36) NOT NULL, \`payerId\` varchar(255) NOT NULL, \`email\` varchar(255) NOT NULL, \`refreshToken\` varchar(255) NOT NULL, \`emailVerified\` tinyint NOT NULL, \`verifiedAccount\` tinyint NOT NULL, PRIMARY KEY (\`id\`)) ENGINE=InnoDB`);
        await queryRunner.query(`CREATE TABLE \`users\` (\`id\` varchar(36) NOT NULL, \`createdAt\` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6), \`name\` varchar(255) NOT NULL, \`email\` varchar(255) NOT NULL, \`hashedPassword\` varchar(255) NULL, \`emailConfirmed\` tinyint NOT NULL DEFAULT 0, \`emailConfirmationToken\` varchar(255) NULL, \`emailConfirmationTokenExpiryTime\` datetime NULL, \`passwordResetToken\` varchar(255) NULL, \`passwordResetTokenExpiryTime\` datetime NULL, \`lockoutEnd\` datetime NULL, \`role\` enum ('admin', 'instructor', 'student') NOT NULL DEFAULT 'student', \`thumbnailId\` varchar(255) NULL, \`thumbnailUrl\` varchar(255) NULL, \`headline\` varchar(255) NULL, \`description\` text NULL, \`websiteUrl\` varchar(255) NULL, \`youtubeUrl\` varchar(255) NULL, \`facebookUrl\` varchar(255) NULL, \`linkedInUrl\` varchar(255) NULL, \`enrolledCoursesVisible\` tinyint NOT NULL DEFAULT 1, \`isPublic\` tinyint NOT NULL DEFAULT 1, \`courseTags\` varchar(255) NOT NULL DEFAULT '', \`categoryIds\` varchar(255) NOT NULL DEFAULT '', \`totalEnrollmentCount\` int NOT NULL DEFAULT '0', \`paypalAccountId\` varchar(36) NULL, INDEX \`IDX_ace513fa30d485cfd25c11a9e4\` (\`role\`), UNIQUE INDEX \`IDX_97672ac88f789774dd47f7c8be\` (\`email\`), UNIQUE INDEX \`REL_2a06e61d5000a2f944396aff68\` (\`paypalAccountId\`), PRIMARY KEY (\`id\`)) ENGINE=InnoDB`);
        await queryRunner.query(`ALTER TABLE \`refresh_tokens\` ADD CONSTRAINT \`FK_610102b60fea1455310ccd299de\` FOREIGN KEY (\`userId\`) REFERENCES \`users\`(\`id\`) ON DELETE CASCADE ON UPDATE NO ACTION`);
        await queryRunner.query(`ALTER TABLE \`users\` ADD CONSTRAINT \`FK_2a06e61d5000a2f944396aff689\` FOREIGN KEY (\`paypalAccountId\`) REFERENCES \`paypal_accounts\`(\`id\`) ON DELETE SET NULL ON UPDATE NO ACTION`);
    }

    public async down(queryRunner: QueryRunner): Promise<void> {
        await queryRunner.query(`ALTER TABLE \`users\` DROP FOREIGN KEY \`FK_2a06e61d5000a2f944396aff689\``);
        await queryRunner.query(`ALTER TABLE \`refresh_tokens\` DROP FOREIGN KEY \`FK_610102b60fea1455310ccd299de\``);
        await queryRunner.query(`DROP INDEX \`REL_2a06e61d5000a2f944396aff68\` ON \`users\``);
        await queryRunner.query(`DROP INDEX \`IDX_97672ac88f789774dd47f7c8be\` ON \`users\``);
        await queryRunner.query(`DROP INDEX \`IDX_ace513fa30d485cfd25c11a9e4\` ON \`users\``);
        await queryRunner.query(`DROP TABLE \`users\``);
        await queryRunner.query(`DROP TABLE \`paypal_accounts\``);
        await queryRunner.query(`DROP TABLE \`refresh_tokens\``);
    }

}
