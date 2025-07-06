CREATE TABLE inventory_items (
    id UUID UNIQUE DEFAULT gen_random_uuid(),
    product TEXT NOT NULL,
    quantity_available INTEGER NOT NULL,

    product_external_id UUID NOT NULL UNIQUE,

    delete_at TIMESTAMPTZ NULL,
    update_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),

    CONSTRAINT pk_inventory_items PRIMARY KEY (id)
);
