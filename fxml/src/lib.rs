pub mod settings;
pub mod conversion;

pub fn run(input: &String, config: &settings::config::Config) -> Result<(), Box<dyn std::error::Error>> {
    if config.help() {
        println!("{}", help_text());
        return Ok(());
    }

    let result = conversion::xml_conversion::convert(&input);
    print!("Result:\n{result}");

    Ok(())
}

fn version() -> &'static str {
    env!("CARGO_PKG_VERSION")
}

fn help_text() -> String {
    format!("fxml (format-xml) version {}", version())
}

#[cfg(test)]
mod tests {
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
}