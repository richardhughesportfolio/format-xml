use xmlparser;

pub fn convert(input: &str) -> String {
    let mut output = String::new();
    output.reserve(input.len() * 2);

    let mut indent = 0;

    for token in xmlparser::Tokenizer::from(input) {
        match token {
            Err(_) => return String::from(input),
            Ok(t) => match t {
                // xmlparser::Token::Declaration { version, encoding, standalone, span } => todo!(),
                // xmlparser::Token::ProcessingInstruction { target, content, span } => todo!(),
                // xmlparser::Token::Comment { text, span } => todo!(),
                // xmlparser::Token::DtdStart { name, external_id, span } => todo!(),
                // xmlparser::Token::EmptyDtd { name, external_id, span } => todo!(),
                // xmlparser::Token::EntityDeclaration { name, definition, span } => todo!(),
                // xmlparser::Token::DtdEnd { span } => todo!(),
                xmlparser::Token::ElementStart { prefix, local, span } => {
                    output.push_str(&span)
                },
                // xmlparser::Token::Attribute { prefix, local, value, span } => todo!(),
                xmlparser::Token::ElementEnd { end, span } => {
                    match end {
                        xmlparser::ElementEnd::Open => output.push_str(">\n"),
                        xmlparser::ElementEnd::Close(_, _) => output.push_str(&span),
                        xmlparser::ElementEnd::Empty => output.push_str(&span),
                    }
                },
                // xmlparser::Token::Text { text } => todo!(),
                // xmlparser::Token::Cdata { text, span } => todo!(),
                _ => continue,
            },
        };
    }

    output
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn convert_empty_input_returns_empty_result() {
        let input = "";

        let result = convert(input);

        assert_eq!("", result);
    }

    #[test]
    fn convert_invalid_input_returns_input() {
        let input = "invalid XML...";

        let result = convert(input);

        assert_eq!(input, result);
    }

    #[test]
    fn convert_valid_input_returns_formatted_input() {
        let input = r#"<tag>
</tag>"#;

        let result = convert(input);
        println!("{result}");

        assert_eq!(input, result);
    }
}
