<?php
/**
 * Plugin Name: Unity Data Receiver
 * Description: Receives data from Unity and stores it in a custom table.
 */

// Создание таблицы при активации плагина
function unity_data_receiver_install() {
    global $wpdb;
    $table_name = $wpdb->prefix . 'unity_data';
    $charset_collate = $wpdb->get_charset_collate();

    $sql = "CREATE TABLE $table_name (
        id mediumint(9) NOT NULL AUTO_INCREMENT,
        name text NOT NULL,
        PRIMARY KEY  (id)
    ) $charset_collate;";

    require_once( ABSPATH . 'wp-admin/includes/upgrade.php' );
    dbDelta( $sql );
}
register_activation_hook( __FILE__, 'unity_data_receiver_install' );

// Регистрация REST API эндпойнта
add_action('rest_api_init', function () {
    register_rest_route('unity/v1', '/data', array(
        'methods' => 'POST',
        'callback' => 'unity_data_receiver_handle_post',
        'permission_callback' => '__return_true',
    ));
});

// Обработка POST-запроса и сохранение данных
function unity_data_receiver_handle_post(WP_REST_Request $request) {
    global $wpdb;
    $table_name = $wpdb->prefix . 'unity_data';

    $name = sanitize_text_field($request->get_param('name'));
    $time = sanitize_text_field($request->get_param('time'));
    $data = sanitize_text_field($request->get_param('data'));

    $wpdb->insert(
        $table_name,
        array(
            'name' => $name,
            'time' => $time,
            'data' => $data
        )
    );

    return new WP_REST_Response('Data received', 200);
}
?>
