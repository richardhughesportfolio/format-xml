pub mod settings;
pub mod conversion;

pub fn run(input: &String, config: &settings::config::Config) -> Result<String, Box<dyn std::error::Error>> {
    if config.help() {
        return Ok(help_text());
    }

    Ok(conversion::xml_conversion::convert(&input))
}

fn version() -> &'static str {
    env!("CARGO_PKG_VERSION")
}

fn help_text() -> String {
    format!("fxml (format-xml) version {}", version())
}

#[cfg(test)]
mod tests {
    use crate::conversion::xml_conversion::convert;

    use super::*;

    #[test]
    fn version_returns_value() {
        let result = version();
        assert!(!result.is_empty());
    }

    #[test]
    fn help_text_returns_value() {
        let result = help_text();
        assert!(!result.is_empty());
    }

    #[test]
    fn run_display_help_text_returns_help_text() {
        let input = String::from("text");
        let config = settings::config::Config::new(&vec![String::from("--help")]);

        let result = run(&input, &config);

        assert!(result.is_ok());
        assert_eq!(help_text(), result.ok().unwrap());
    }

    #[test]
    fn run_converts_text_returns_converted_text() {
        let input = String::from("<text/>");
        let config = settings::config::Config::new(&vec![]);

        let result = run(&input, &config);

        assert!(result.is_ok());
        assert_eq!(convert(&input), result.ok().unwrap());
    }
}
