<?php
if (!defined('ABSPATH')) {
    exit;
}

$data = get_option('unity_json_data', array());

echo '<div class="wrap">';
echo '<h1>Unity JSON Data</h1>';

if (empty($data)) {
    echo '<p>No data received yet.</p>';
} else {
    echo '<table class="widefat fixed" cellspacing="0">';
    echo '<thead><tr>';

    // Заголовки таблицы на основе ключей первого элемента массива
    foreach (array_keys($data[0]) as $key) {
        echo '<th>' . esc_html($key) . '</th>';
    }

    echo '</tr></thead>';
    echo '<tbody>';

    // Данные таблицы
    foreach ($data as $entry) {
        echo '<tr>';
        foreach ($entry as $value) {
            echo '<td>' . esc_html($value) . '</td>';
        }
        echo '</tr>';
    }

    echo '</tbody>';
    echo '</table>';
}

echo '</div>';
?>
