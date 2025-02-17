extends Node3D

@onready var leftPage: MeshInstance3D = $Book/Mesh_PageL
@onready var rightPage: MeshInstance3D = $Book/Mesh_PageR
@onready var pivot: Node3D = $Book/Pivot

var T_page_blank = preload("res://Assets/Textures/Pages/blank.png")
var T_page_cover = preload("res://Assets/Textures/Pages/FrontCover.png")
var T_page_01 = preload("res://Assets/Textures/Pages/Page_01.png")
var T_page_02 = preload("res://Assets/Textures/Pages/Page_02.png")
var T_page_03 = preload("res://Assets/Textures/Pages/Page_03.png")
var T_page_04 = preload("res://Assets/Textures/Pages/Page_04.png")

# Called when the node enters the scene tree for the first time.
func _ready():
	var leftPageTextPlane = leftPage.get_child(0)
	if leftPageTextPlane and leftPageTextPlane is MeshInstance3D:
		var mat = leftPageTextPlane.get_surface_override_material(0)
		
		if mat and mat is ShaderMaterial:
			mat.set_shader_parameter("tex_frg_2", T_page_blank)
	
	var rightPageTextPlane = rightPage.get_child(0)
	if rightPageTextPlane and rightPageTextPlane is MeshInstance3D:
		var mat = rightPageTextPlane.get_surface_override_material(0)
		
		if mat and mat is ShaderMaterial:
			mat.set_shader_parameter("tex_frg_2", T_page_cover)



# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
