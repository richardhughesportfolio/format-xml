# Builds the application. The output is in `/src/build/`. The final binary name will be `fxml`.
# The binary will be tested, then compressed and stored in `/packages/`.

import os
import sys
import shutil
import tarfile
import argparse
import platform
import subprocess

dotnet_path = shutil.which("dotnet")

def configure_arguments():
    parser = argparse.ArgumentParser()
    parser.add_argument("-a", "--arch", action="store", choices=["amd64", "arm64"], required=False, default="amd64", help="the target architecture")

    return parser.parse_args()


def run_cmd(cmd, stdin=""):
    print(f"Running command: '{cmd}'")

    try:
        process = subprocess.run(cmd, check=True, input=stdin, capture_output=True)
        return process.stdout.decode()
    except Exception as err:
        print(f"Failed to run command: '{cmd}' with error: '{err}'.")
        sys.exit(1)


def get_version():
    # the version is expected to be on the first line of this file
    version_file_path = os.path.join(os.getcwd(), "src", "version.txt")
    with open(version_file_path) as fp:
        return fp.readline()


def get_target_architecture(arch):
    if arch == "amd64":
        return "x64"
    elif arch == "arm64":
        return "arm64"
    else:
        print(f"Unsupported architecture: {arch}")
        sys.exit(1)


def build(arch):
    print("Building...")

    build_dir = os.path.join(os.getcwd(), "src", "build")

    version = get_version()

    target_architecture = get_target_architecture(arch)

    # see https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish
    build_cmd = [dotnet_path,
           "publish",
           "./src/FormatXML/",
           "--configuration=release",
           f"--output={build_dir}",
           "--self-contained", "true",
           f"--arch={target_architecture}",
           f"/p:Version={version}"]
    run_cmd(build_cmd)

    original_binary_path = os.path.join(build_dir, "FormatXML")
    new_binary_ptah = os.path.join(build_dir, "fxml")
    shutil.move(original_binary_path, new_binary_ptah)

    return new_binary_ptah


def test(binary_path):
    print("Testing...")

    test_correct_formatting(binary_path)
    test_correct_exit_code_on_failure_with_strict_mode(binary_path)


def test_correct_formatting(binary_path):
    print("Test correct formatting...")

    test_cmd = [binary_path]

    test_input = "<tag/>"
    output = run_cmd(test_cmd, stdin=test_input.encode())

    if (output != "<tag />"):
        print(f"Binary `{binary_path}` did not produce correct output: `{output}`.")
        sys.exit(1)


def test_correct_exit_code_on_failure_with_strict_mode(binary_path):
    print("Test correct exit code on failure with strict mode...")

    # we want this command to fail, so we can't use `run_cmd()`
    try:
        test_cmd = [binary_path, "--strict"]
        test_input = "<invalid xml..."

        subprocess.run(test_cmd, check=True, input=test_input.encode(), capture_output=True)
    except Exception:
        return

    print(f"Binary `{binary_path}` did not produce correct error code on failure with strict mode.")
    sys.exit(1)


def package(binary_path, arch):
    print("Packaging...")

    packages_path = os.path.join(os.getcwd(), "packages")
    os.makedirs(packages_path, exist_ok=True)

    package_name = get_package_name(arch)

    package_path = os.path.join(packages_path, package_name)
    with tarfile.open(package_path, "w:gz") as tar:
        tar.add(binary_path, arcname="fxml")

    print(f"Package created at: `{package_path}`.")


def get_package_name(arch):
    platform_name = platform.system()

    os_name = ""

    if platform_name == "Darwin":
        os_name = "darwin"
    elif platform_name == "Windows":
        os_name = "windows"
    elif platform_name == "Linux":
        os_name = "linux"
    else:
        print(f"Unsupported platform: {platform_name}")
        sys.exit(1)

    return f"fxml_{os_name}_{arch}.tar.gz"


print("Building format-xml...")

args = configure_arguments()

binary_path = build(args.arch)

test(binary_path)

package(binary_path, args.arch)

print("Built format-xml.")
