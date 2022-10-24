use fxml::settings::config;

fn main() {
    let args = std::env::args().collect();

    let config = config::Config::new(&args);

    if let Err(e) = fxml::run(&config) {
        eprintln!("An error occurred when running fxml: {e}");

        if config.strict() {
            std::process::exit(1);
        }
    }
}
