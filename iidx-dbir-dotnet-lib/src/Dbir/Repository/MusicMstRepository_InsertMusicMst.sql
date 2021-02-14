INSERT INTO music_mst (
	  name
	, play_style
	, charts_type
	, level
	, genre
	, composer
	, min_bpm
	, max_bpm
	, all_notes_num
	, scratch_num
	, charge_note_num
	, back_spin_scratch_num
	, create_datetime
	, update_datetime
) VALUES (
	  /*Name*/'V2'
	, /*PlayStyle*/'SP'
	, /*ChartsType*/'ANOTHER'
	, /*Level*/12
	, /*Genre*/'Techno'
	, /*Composer*/'TAKA'
	, /*MinBPM*/149
	, /*MaxBPM*/150
	, /*AllNotesNum*/1234
	, /*ScratchNum*/56
	, /*ChargeNoteNum*/7
	, /*BackSpinScratchNum*/8
	, CURRENT_TIMESTAMP
	, null
);