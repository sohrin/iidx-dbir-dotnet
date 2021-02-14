SELECT
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
FROM
	music_mst
WHERE
	name = /*Name*/'V'
AND play_style = /*PlayStyle*/'SP'
AND charts_type = /*ChartsType*/'HYPER'
;