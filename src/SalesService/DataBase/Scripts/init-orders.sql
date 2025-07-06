CREATE EXTENSION IF NOT EXISTS "pgcrypto";

CREATE TABLE IF NOT EXISTS orders (
    id UUID UNIQUE DEFAULT gen_random_uuid(),
    product TEXT NOT NULL,
    quantity INTEGER NOT NULL,
    total_price NUMERIC(18, 2) NOT NULL,
    product_external_id UUID,
    delete_at TIMESTAMPTZ NULL,
    update_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    CONSTRAINT pk_orders PRIMARY KEY (id)
);
