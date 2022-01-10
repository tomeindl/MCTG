create table mctg_users
(
    users_id      serial
        constraint mctg_users_pk
            primary key,
    users_name    varchar not null,
    users_pw      varchar not null,
    users_gold    integer default 20,
    users_elo     integer default 300,
    users_admin   boolean default false,
    users_bio     text,
    users_picture varchar,
    users_wins    integer default 0,
    users_losses  integer default 0
);

alter table mctg_users
    owner to swe1user;

create unique index mctg_users_users_id_uindex
    on mctg_users (users_id);

create unique index mctg_users_users_name_uindex
    on mctg_users (users_name);

create table mctg_sessions
(
    sessions_user  integer   not null
        constraint mctg_sessions_pk
            primary key
        constraint mctg_sessions_mctg_users_users_id_fk
            references mctg_users
            on delete cascade,
    sessions_token varchar   not null,
    sessions_time  timestamp not null
);

alter table mctg_sessions
    owner to swe1user;

create unique index mctg_sessions_sessions_user_uindex
    on mctg_sessions (sessions_user);

create table mctg_cards
(
    cards_id       serial
        constraint mctg_cards_pk
            primary key,
    cards_name     varchar not null,
    cards_type     varchar not null,
    cards_element  integer not null,
    cards_race     integer,
    cards_dmg      integer,
    cards_trap     varchar,
    cards_rarity   integer not null,
    cards_value    integer not null,
    cards_location varchar not null
);

alter table mctg_cards
    owner to swe1user;

create unique index mctg_cards_cards_id_uindex
    on mctg_cards (cards_id);

create table mctg_decks
(
    decks_id   serial
        constraint mctg_decks_pk
            primary key,
    decks_user integer not null
        constraint mctg_decks_mctg_users_users_id_fk
            references mctg_users,
    decks_name varchar not null
);

alter table mctg_decks
    owner to swe1user;

create table mctg_usercards
(
    usercards_user integer not null
        constraint mctg_usercards_mctg_users_users_id_fk
            references mctg_users,
    usercards_card integer not null
        constraint mctg_usercards_pk
            primary key
        constraint mctg_usercards_mctg_cards_cards_id_fk
            references mctg_cards,
    usercards_deck integer
        constraint mctg_usercards_mctg_decks_decks_id_fk
            references mctg_decks
);

alter table mctg_usercards
    owner to swe1user;

create unique index mctg_usercards_usercards_card_uindex
    on mctg_usercards (usercards_card);

create unique index mctg_decks_decks_id_uindex
    on mctg_decks (decks_id);

create table mctg_offers
(
    offers_id   serial
        constraint mctg_offers_pk
            primary key,
    offers_user integer not null
        constraint mctg_offers_mctg_users_users_id_fk
            references mctg_users,
    offers_card integer not null
        constraint mctg_offers_mctg_cards_cards_id_fk
            references mctg_cards
);

alter table mctg_offers
    owner to swe1user;

create unique index mctg_offers_offers_id_uindex
    on mctg_offers (offers_id);


