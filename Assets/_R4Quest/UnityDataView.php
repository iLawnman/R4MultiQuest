function display_unity_data_table() {
    global $wpdb;
    $table_name = $wpdb->prefix . 'unity_data';

    $results = $wpdb->get_results("SELECT * FROM $table_name");

    if (!empty($results)) {
        echo '<table>';
        echo '<tr><th>ID</th><th>Name</th><th>Time</th><th>Data</th></tr>';
        foreach ($results as $row) {
            echo '<tr>';
            echo '<td>' . esc_html($row->id) . '</td>';
            echo '<td>' . esc_html($row->name) . '</td>';
            echo '<td>' . esc_html($row->time) . '</td>';
            echo '<td>' . esc_html($row->data) . '</td>';
            echo '</tr>';
        }
        echo '</table>';
    } else {
        echo 'No data found.';
    }
}

add_shortcode('unity_data_table', 'display_unity_data_table');
