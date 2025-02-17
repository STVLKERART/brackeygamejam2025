extends Node3D

@onready var leftPage: MeshInstance3D = $Book/Mesh_PageL
@onready var rightPage: MeshInstance3D = $Book/Mesh_PageR
@onready var pivot: Node3D = $Book/Pivot

# Called when the node enters the scene tree for the first time.
func _ready():
	var new_texture = preload("res://Assets/Textures/Pages/testPag.png")
	
	var leftPageTextPlane = leftPage.get_child(0)
	if leftPageTextPlane and leftPageTextPlane is MeshInstance3D:
		var mat1 = leftPageTextPlane.get_surface_override_material(0)
		
		if mat1 and mat1 is ShaderMaterial:
			mat1.set_shader_parameter("tex_frg_2", new_texture)
			print("yum")
			print (new_texture)
			print (leftPageTextPlane)



# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
