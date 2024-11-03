CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;

CREATE TABLE audit_trail_logs (
    id uuid NOT NULL,
    data jsonb NOT NULL,
    action_type text NOT NULL,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    CONSTRAINT audit_trail_logs_pk PRIMARY KEY (id)
);

CREATE TABLE products (
    id uuid NOT NULL,
    name text NOT NULL,
    price numeric NOT NULL,
    qty integer NOT NULL,
    deleted_at timestamp with time zone,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    created_by text NOT NULL DEFAULT ('system'::text),
    updated_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    updated_by text NOT NULL DEFAULT ('system'::text),
    CONSTRAINT products_pk PRIMARY KEY (id)
);
COMMENT ON TABLE products IS 'Store the product list for sale.';
COMMENT ON COLUMN products.id IS 'Store product ID as primary key.';
COMMENT ON COLUMN products.created_at IS 'Store the created date & time of the data.';
COMMENT ON COLUMN products.created_by IS 'Store the name of who created the data.';

CREATE TABLE user_accounts (
    id uuid NOT NULL,
    full_name text NOT NULL,
    deleted_at timestamp with time zone,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    created_by text NOT NULL DEFAULT ('system'::text),
    updated_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    updated_by text NOT NULL DEFAULT ('system'::text),
    CONSTRAINT user_accounts_pk PRIMARY KEY (id)
);

CREATE TABLE cart_items (
    user_account_id uuid NOT NULL,
    product_id uuid NOT NULL,
    qty integer NOT NULL,
    CONSTRAINT cart_items_pk PRIMARY KEY (user_account_id, product_id),
    CONSTRAINT cart_items__products_fk FOREIGN KEY (product_id) REFERENCES products (id),
    CONSTRAINT cart_items__user_accounts FOREIGN KEY (user_account_id) REFERENCES user_accounts (id)
);

CREATE TABLE receipts (
    id uuid NOT NULL,
    user_account_id uuid,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    created_by text NOT NULL DEFAULT ('system'::text),
    CONSTRAINT receipts_pk PRIMARY KEY (id),
    CONSTRAINT receipts__account_id_fk FOREIGN KEY (user_account_id) REFERENCES user_accounts (id)
);

CREATE TABLE receipt_details (
    id uuid NOT NULL,
    receipt_id uuid NOT NULL,
    product_id uuid NOT NULL,
    price numeric NOT NULL,
    qty integer NOT NULL,
    CONSTRAINT receipt_details_pk PRIMARY KEY (id),
    CONSTRAINT receipt_details__receipts_fk FOREIGN KEY (receipt_id) REFERENCES receipts (id)
);

CREATE INDEX ix_cart_items_product_id ON cart_items (product_id);

CREATE INDEX ix_receipt_details_receipt_id ON receipt_details (receipt_id);

CREATE INDEX ix_receipts_user_account_id ON receipts (user_account_id);

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20241103080530_Initial', '8.0.10');

COMMIT;

