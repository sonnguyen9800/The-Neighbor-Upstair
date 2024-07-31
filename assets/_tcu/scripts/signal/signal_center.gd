extends Node

signal click_mask


func SendSignalClickMask():
    click_mask.emit()