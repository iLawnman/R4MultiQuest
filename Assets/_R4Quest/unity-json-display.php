<?php
/*
Plugin Name: Unity JSON Display
Description: Прием данных JSON из Unity и отображение их в таблице на странице WordPress.
Version: 1.0
Author: Your Name
*/

// Защита от прямого доступа
if (!defined('ABSPATH')) {
    exit;
}

// Хук для регистрации страницы администратора
add_action('admin_menu', 'unity_json_display_menu');

function unity_json_display_menu() {
    add_menu_page('Unity JSON Display', 'Unity JSON', 'manage_options', 'unity-json-display', 'unity_json_display_page');
}

// Обработчик страницы администратора
function unity_json_display_page() {
    include(plugin_dir_path(__FILE__) . 'includes/admin-page.php');
}

// Регистрация endpoint для приема POST-запросов
add_action('rest_api_init', function () {
    register_rest_route('unity-json/v1', '/submit', array(
        'methods' => 'POST',
        'callback' => 'unity_json_display_handle_post',
    ));
});

function unity_json_display_handle_post(WP_REST_Request $request) {
    $data = $request->get_json_params();

    // Получаем существующие данные
    $existing_data = get_option('unity_json_data', array());

    // Добавляем новые данные
    $existing_data[] = $data;

    // Сохраняем обновленные данные
    update_option('unity_json_data', $existing_data);

    return new WP_REST_Response('Data received', 200);
}
