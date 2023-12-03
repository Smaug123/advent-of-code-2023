{
    description = "Advent of Code 2023";
    inputs = {
        flake-utils.url = "github:numtide/flake-utils";
        nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    };

    outputs = {
        self,
        nixpkgs,
        flake-utils,
    }: flake-utils.lib.eachDefaultSystem (system:
    let pkgs = nixpkgs.legacyPackages.${system}; in
    {
        devShells = { default = pkgs.mkShell {
          buildInputs = with pkgs; [
            (with dotnetCorePackages;
              combinePackages [
                dotnet-sdk_8
                dotnetPackages.Nuget
              ])
          ] ++ [pkgs.swift darwin.apple_sdk.frameworks.Foundation darwin.apple_sdk.frameworks.CryptoKit darwin.apple_sdk.frameworks.GSS pkgs.zlib pkgs.zlib.dev pkgs.openssl pkgs.icu];
        };};
    }
    );
}
