CREATE TABLE user_accounts
(
	id UUID
		CONSTRAINT user_accounts_pk PRIMARY KEY,
		
	full_name TEXT NOT NULL,
	
	deleted_at TIMESTAMPTZ NULL,
	
	created_at TIMESTAMPTZ NOT NULL
		DEFAULT CURRENT_TIMESTAMP,
	created_by TEXT NOT NULL
		DEFAULT 'system',
	updated_at TIMESTAMPTZ NOT NULL
		DEFAULT CURRENT_TIMESTAMP,
	updated_by TEXT NOT NULL
		DEFAULT 'system'
);

CREATE TABLE products
(
	id UUID 
		CONSTRAINT products_pk PRIMARY KEY,
		
	"name" TEXT NOT NULL,
	
	price DECIMAL NOT NULL,
	
	qty INT NOT NULL,
	
	deleted_at TIMESTAMPTZ NULL,
	
	created_at TIMESTAMPTZ NOT NULL
		DEFAULT CURRENT_TIMESTAMP,
	created_by TEXT NOT NULL
		DEFAULT 'system',
	updated_at TIMESTAMPTZ NOT NULL
		DEFAULT CURRENT_TIMESTAMP,
	updated_by TEXT NOT NULL
		DEFAULT 'system'
);

CREATE TABLE cart_items
(
	user_account_id UUID
		CONSTRAINT cart_items__user_accounts REFERENCES user_accounts,
	product_id UUID
		CONSTRAINT cart_items__products_fk REFERENCES products,
	
	CONSTRAINT cart_items_pk PRIMARY KEY (user_account_id, product_id),
	
	qty INT NOT NULL
);

CREATE TABLE receipts
(
	id UUID
		CONSTRAINT receipts_pk PRIMARY KEY,
		
	user_account_id UUID
		CONSTRAINT receipts__account_id_fk REFERENCES user_accounts,
	
	created_at TIMESTAMPTZ NOT NULL
		DEFAULT CURRENT_TIMESTAMP,
	created_by TEXT NOT NULL
		DEFAULT 'system'
);

CREATE TABLE receipt_details
(
	id UUID
		CONSTRAINT receipt_details_pk PRIMARY KEY,
		
	receipt_id UUID NOT NULL
		CONSTRAINT receipt_details__receipts_fk REFERENCES receipts,
		
	product_id UUID NOT NULL
		CONSTRAINT receipt_details__products_fk REFERENCES products,
		
	price DECIMAL NOT NULL,
	qty INT NOT NULL
);

CREATE TABLE audit_trail_logs
(
	id UUID
		CONSTRAINT audit_trail_logs_pk PRIMARY KEY,

	data JSONB NOT NULL,

	action_type TEXT NOT NULL,

	created_at TIMESTAMPTZ NOT NULL
		DEFAULT CURRENT_TIMESTAMP
);