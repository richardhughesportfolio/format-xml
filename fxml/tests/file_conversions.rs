use fxml::{self, settings::config};

fn test_files() -> Vec<(&'static str, &'static str)> {
    vec![("tests/data/1_expected.xml", "tests/data/1.xml")]
}

#[test]
fn files_convert_correctly() {
    let config = config::Config::new(&vec![]);

    for (expected_file_name, input_file_name) in test_files() {
        let expected = std::fs::read_to_string(expected_file_name)
            .expect("Failed to find test file: {expected_file_name}.");

        let input = std::fs::read_to_string(input_file_name)
            .expect("Failed to find test file: {input_file_name}.");

        match fxml::run(&input, &config) {
            Ok(output) => assert_eq!(expected, output),
            Err(e) => {
                eprintln!("An error occurred when running fxml: {e}");
        
                if config.strict() {
                    std::process::exit(1);
                }
            },
        }
    }
}
