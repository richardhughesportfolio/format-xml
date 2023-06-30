# Builds the application. The output is in `/src/build/`. The final binary name will be `fxml`.
# The binary will be tested, then compressed and stored in `/packages/`.

import os
import sys
import shutil
import tarfile
import subprocess

dotnet_path = shutil.which("dotnet")

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


def build():
    print("Building...")

    build_dir = os.path.join(os.getcwd(), "src", "build")

    version = get_version()

    # see https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish
    build_cmd = [dotnet_path,
           "publish",
           "./src/FormatXML/",
           "--configuration=release",
           f"--output={build_dir}",
           "--self-contained", "true",
           f"/p:Version={version}"]
    run_cmd(build_cmd)

    original_binary_path = os.path.join(build_dir, "FormatXML")
    new_binary_ptah = os.path.join(build_dir, "fxml")
    shutil.move(original_binary_path, new_binary_ptah)

    return new_binary_ptah


def test(binary_path):
    print("Testing...")

    test_cmd = [binary_path]

    test_input = "<tag/>"
    output = run_cmd(test_cmd, stdin=test_input.encode())

    if (output != "<tag />"):
        print(f"Binary `{binary_path}` did not produce correct output: `{output}`.")
        sys.exit(1)


def package(binary_path):
    print("Packaging...")

    packages_path = os.path.join(os.getcwd(), "packages")
    os.makedirs(packages_path)

    package_path = os.path.join(packages_path, "fxml.tar.gz")
    with tarfile.open(package_path, "w:gz") as tar:
        tar.add(binary_path, arcname="fxml")

    print(f"Package created at: `{package_path}`.")


print("Building format-xml...")

binary_path = build()

test(binary_path)

package(binary_path)

print("Built format-xml.")
