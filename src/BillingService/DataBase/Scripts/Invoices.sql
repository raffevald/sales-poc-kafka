-- Criação da tabela "invoices"
CREATE TABLE invoices (
    id UUID UNIQUE DEFAULT gen_random_uuid(),

    sale_external_id UUID NOT NULL,
    product TEXT NOT NULL,
    amount NUMERIC(18, 2) NOT NULL,
    issued_at TIMESTAMPTZ NOT NULL DEFAULT now(),

    delete_at TIMESTAMPTZ NULL,
    update_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),

    CONSTRAINT pk_invoices PRIMARY KEY (id)
);
