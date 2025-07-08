CREATE TABLE client_webhooks (
    id UUID UNIQUE DEFAULT gen_random_uuid(),
    client_id UUID NOT NULL,
    url TEXT NOT NULL,
    active BOOLEAN DEFAULT true,
    event_type TEXT NOT NULL, -- ex: 'sale.created'

    delete_at TIMESTAMPTZ NULL,
    update_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),

    CONSTRAINT pk_client_webhooks PRIMARY KEY (id)
);
