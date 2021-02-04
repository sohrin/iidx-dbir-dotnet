CREATE DATABASE dbir_db COLLATE Japanese_XJIS_140_CI_AS_VSS_UTF8;
go

use dbir_db;
go

CREATE TABLE music_mst (
      name varchar(100)
    , play_style varchar(2)
    , charts_type varchar(11)
    , level int
    , genre  varchar(30)
    , composer varchar(100)
    , min_bpm int
	, max_bpm int
	, all_notes_num int
	, scratch_num int
	, charge_note_num int
    , back_spin_scratch_num int
    , create_datetime datetime
    , update_datetime datetime
    primary key(name, play_style, charts_type)
);
GO