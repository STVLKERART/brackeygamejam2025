extends Node3D

@onready var MI_leftPage: ShaderMaterial = $Book/Mesh_PageL.get_child(0).get_surface_override_material(0)
@onready var MI_rightPage: ShaderMaterial = $Book/Mesh_PageR.get_child(0).get_surface_override_material(0)
@onready var pivot: Node3D = $Book/Pivot
@onready var MI_mobilePageText_Front: ShaderMaterial = pivot.get_child(0).get_child(0).get_surface_override_material(0)
@onready var MI_mobilePageText_Back: ShaderMaterial = pivot.get_child(1).get_child(0).get_surface_override_material(0)

var T_page_blank = preload("res://Assets/Textures/Pages/blank.png")
var T_pages: Array[CompressedTexture2D] = [
	preload("res://Assets/Textures/Pages/FrontCover.png"), 
	preload("res://Assets/Textures/Pages/Page_01.png"), 
	preload("res://Assets/Textures/Pages/Page_02.png"), 
	preload("res://Assets/Textures/Pages/Page_03.png"), 
	preload("res://Assets/Textures/Pages/Page_04.png")
	]

@export var rotation_time: float = .4
var current_page: int = 0
var rotating: bool = false

# Called when the node enters the scene tree for the first time.
func _ready():
	if MI_leftPage:
		MI_leftPage.set_shader_parameter("tex_frg_2", T_page_blank)
	
	if MI_rightPage:
		MI_rightPage.set_shader_parameter("tex_frg_2", T_pages[0])


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass


func _input(event):
	if event is InputEventKey and event.pressed and event.keycode == KEY_P:
		if not rotating and current_page + 1 < T_pages.size():
			start_fwd_rotation(rotation_time)
	elif event is InputEventKey and event.pressed and event.keycode == KEY_O:
		if not rotating and current_page > 0:
			start_bkwd_rotation(rotation_time)

func start_fwd_rotation(duration: float):
	rotating = true
	set_mobile_page_textures() # Update page textures
	await rotate_over_time(Vector3.ZERO, Vector3(0, deg_to_rad(-140 * 0.9), 0), duration * 0.9)
	update_page_textures()
	await rotate_over_time(Vector3(0, deg_to_rad(-140 * 0.9), 0), Vector3(0, deg_to_rad(-140), 0), duration * 0.1)
	rotating = false
	current_page += 2
	

func start_bkwd_rotation(duration: float):
	rotating = true
	set_mobile_page_textures_BKWD() # Update page textures
	await rotate_over_time(Vector3(0, deg_to_rad(-140), 0),Vector3(0, deg_to_rad(-140 * 0.1), 0), duration * 0.9)
	update_page_textures_BKWD()
	await rotate_over_time(Vector3(0, deg_to_rad(-140 * 0.1), 0), Vector3.ZERO, duration * 0.1)
	rotating = false
	if current_page - 2 >= 0:
		current_page -= 2
	else:
		current_page = 0
	

func rotate_over_time(start_rotation: Vector3, target_rotation: Vector3, duration: float):
	var elapsed_time = 0.0
	
	while elapsed_time < duration:
		elapsed_time += get_physics_process_delta_time()
		var t = elapsed_time / duration
		pivot.rotation = start_rotation.lerp(target_rotation, t)
		await get_tree().process_frame  # Yield execution until the next frame

	pivot.rotation = target_rotation  # Ensure final rotation is exact

func set_mobile_page_textures():
	if MI_mobilePageText_Front and current_page < T_pages.size():
		MI_mobilePageText_Front.set_shader_parameter("tex_frg_2", T_pages[current_page])
	
	if MI_mobilePageText_Back and current_page + 1 < T_pages.size():
		MI_mobilePageText_Back.set_shader_parameter("tex_frg_2", T_pages[current_page + 1])
	
	if MI_rightPage and current_page + 2 < T_pages.size():
		MI_rightPage.set_shader_parameter("tex_frg_2", T_pages[current_page + 2])

func update_page_textures():
	if MI_leftPage and current_page + 1 < T_pages.size():
		MI_leftPage.set_shader_parameter("tex_frg_2", T_pages[current_page + 1])
		

func set_mobile_page_textures_BKWD():
	if MI_mobilePageText_Front:# and current_page > 0:
		MI_mobilePageText_Front.set_shader_parameter("tex_frg_2", T_pages[current_page - 2])
	
	if MI_mobilePageText_Back:# and current_page - 1 > 0:
		MI_mobilePageText_Back.set_shader_parameter("tex_frg_2", T_pages[current_page - 1])
	
	if MI_leftPage:
		if current_page - 3 > 0:
			MI_leftPage.set_shader_parameter("tex_frg_2", T_pages[current_page - 3])
		else:
			MI_leftPage.set_shader_parameter("tex_frg_2", T_page_blank)

func update_page_textures_BKWD():
	if MI_rightPage:# and current_page - 1 > 0:
		MI_rightPage.set_shader_parameter("tex_frg_2", T_pages[current_page - 2])
