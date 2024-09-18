import { MigrationInterface, QueryRunner } from "typeorm";

export class TokenRefactoring1726655950740 implements MigrationInterface {
    name = 'TokenRefactoring1726655950740'

    public async up(queryRunner: QueryRunner): Promise<void> {
        await queryRunner.query(`CREATE INDEX \`IDX_d417e5d35f2434afc4bd48cb4d\` ON \`tokens\` (\`userId\`)`);
    }

    public async down(queryRunner: QueryRunner): Promise<void> {
        await queryRunner.query(`DROP INDEX \`IDX_d417e5d35f2434afc4bd48cb4d\` ON \`tokens\``);
    }

}
