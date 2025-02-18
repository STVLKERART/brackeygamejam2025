extends Node3D

@onready var emmiter: GPUParticles3D = $GPUParticles3D

func _ready() -> void:
	emmiter.emitting = false
func spray_confetti() -> void:
	emmiter.emitting = true
	await get_tree().create_timer(0.1).timeout
	emmiter.emitting = false
	
